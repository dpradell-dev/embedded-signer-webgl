using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class FirebaseAuthController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void initializeFirebaseApp();

    /*
    [DllImport("__Internal")]
    private static extern void signInWithGoogle();
    */
    
    // Optional UI element to display status or error messages
    public TextMeshProUGUI statusText;

    void Start()
    {
        // Assuming you call your JS lib function to initialize Firebase at some point
        //InitializeFirebase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            //InitializeFirebase();
        }
    }

    private void InitializeFirebase() 
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        // Call the initializeFirebaseApp function from your jslib 
        initializeFirebaseApp(); 
        #endif
    }

    // Called by the WebGL template when Firebase initialization is successful 
    public void OnFirebaseReady()
    {
        Debug.Log("Firebase initialized successfully!");
        if (statusText != null) statusText.text = "Firebase Ready";
    }

    // Called by the WebGL template when Firebase initialization fails
    public void OnFirebaseError(string errorMessage)
    {
        Debug.LogError("Firebase initialization error: " + errorMessage);
        if (statusText != null) statusText.text = "Firebase Error: " + errorMessage; 
    }

    // Called by the WebGL template when a Google sign-in succeeds
    public void OnSignInSuccess(string idToken)
    {
        Debug.Log("Google Sign-In Success! Token: " + idToken);

        // Process the idToken to authenticate the user (Firebase SDK for Unity)
        // ... (Your Firebase authentication logic here)

        if (statusText != null) statusText.text = "Sign-In Success"; 
    }

    // Called by the WebGL template when a Google sign-in fails
    public void OnSignInError(string errorMessage)
    {
        Debug.LogError("Google Sign-In Error: " + errorMessage);
        if (statusText != null) statusText.text = "Sign-In Error: " + errorMessage; 
    }

    // Example function to trigger the Google Sign-In from C#
    public void InitiateGoogleSignIn() 
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        //signInWithGoogle();
        #endif
    }
}
