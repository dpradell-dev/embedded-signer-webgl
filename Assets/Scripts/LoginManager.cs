using System;
using System.Threading.Tasks;
//using GooglePlayGames;
// using GooglePlayGames.BasicApi;
using Newtonsoft.Json;
using Openfort;
using Openfort.Model;
using Openfort.Recovery;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public Button loginButton;
    public Button mintButton;
    public Button openLinkButton;
    
    private string mIdentityToken;
    private string mAccessToken;
    private const string PublishableKey = "pk_test_505bc088-905e-5a43-b60b-4c37ed1f887a";

    private OpenfortSDK mOpenfort;
    private string transactionHash;

    public TextMeshProUGUI statusText;

    void Start()
    {
        //loginButton.onClick.AddListener(OpenfortLogin);
        //mintButton.onClick.AddListener(Mint);
        //GooglePlayLogin();
    }

    private async void GooglePlayLogin ()
    {
        /*
        PlayGamesPlatform.Activate();
        
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success != SignInStatus.Success)
            {
                Debug.Log("Login Failed");
                return;
            }
            
            Debug.Log("Login Success");
            
            PlayGamesPlatform.Instance.RequestServerSideAccess(true, serverAuthCode =>
            {
                var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                
                var credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(serverAuthCode);
                
                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        return;
                    }

                    var user = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);

                    mIdentityToken = user.TokenAsync(false).Result;
                    Debug.Log("Identity Token: " + mIdentityToken);
                });
            });
        });
        */
    }

    // Called from Javascript
    public async void OnSignInSuccess(string idToken)
    {
        Debug.Log("Google Sign-In Success! Token: " + idToken);
        statusText.text = "Google Sign-In Success.";
        
        Debug.Log("Openfort Auth");
        mOpenfort = new OpenfortSDK(PublishableKey); 
        mAccessToken = await mOpenfort.AuthenticateWithOAuth(OAuthProvider.Firebase, idToken);
        Debug.Log("Access Token: " + mAccessToken);

        try
        {
            mOpenfort.ConfigureEmbeddedSigner(80001);
        }
        catch (MissingRecoveryMethod)
        {
            await mOpenfort.ConfigureEmbeddedRecovery(new PasswordRecovery("secret"));
        }
    }
    
    // Called from Javascript
    public void OnSignInError(string errorMessage)
    {
        statusText.text = errorMessage;
        Debug.Log(errorMessage);
    }

    public class RootObject
    {
        public TransactionIntentResponse Data { get; set; }
    }
    public async void OnMintClicked()
    {
        mintButton.interactable = false;
        var webRequest = UnityWebRequest.Post("https://descriptive-night-production.up.railway.app/mint", "");
        webRequest.SetRequestHeader("Authorization", "Bearer " + mAccessToken);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Accept", "application/json");
        await SendWebRequestAsync(webRequest);

        Debug.Log("Mint request sent");
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Mint Failed: " + webRequest.error);
            return;
        }


        var responseText = webRequest.downloadHandler.text;
        Debug.Log("Mint Response: " + responseText);
        var responseJson = JsonConvert.DeserializeObject<RootObject>(responseText);
        var id = responseJson.Data.Id;
        if (responseJson.Data.NextAction == null)
        {
            Debug.Log("No Next Action");
            return;
        }

        var nextAction = responseJson.Data.NextAction.Payload.UserOpHash;

        Debug.Log("Next Action: " + nextAction);
        var intentResponse = await mOpenfort.SendSignatureTransactionIntentRequest(id, nextAction);
        transactionHash = intentResponse.Response.TransactionHash;
        openLinkButton.gameObject.SetActive(true);
        mintButton.interactable = true;
    }
    
    public void OpenLink()
    {
        Application.OpenURL("https://mumbai.polygonscan.com/tx/" + transactionHash);
    }
    
    private Task SendWebRequestAsync(UnityWebRequest webRequest)
    {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        webRequest.SendWebRequest().completed += _ =>
        {
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    tcs.SetResult(true);
                    break;
                default:
                    tcs.SetException(new Exception(webRequest.error));
                    break;
            }
        };
        return tcs.Task;
    }
}
