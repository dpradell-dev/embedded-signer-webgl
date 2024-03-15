mergeInto(LibraryManager.library, {
  initializeFirebaseApp: function() {
    const firebaseConfig = {
      apiKey: "AIzaSyDARPwQjBq0KveTeNHyDSz1p6YeGTHw_Do",
      authDomain: "test-openfort-extension.firebaseapp.com",
      projectId: "test-openfort-extension",
      storageBucket: "test-openfort-extension.appspot.com",
      messagingSenderId: "775020371354",
      appId: "1:775020371354:web:dfc54dfe47f020e184ae23"
    };

    // Initialize Firebase
    const app = initializeApp(firebaseConfig);
  }
});

/*
// MyFirebaseFunctions.jslib 
mergeInto(LibraryManager.library, {
    initializeFirebaseApp: function() {
        const script1 = document.createElement('script');
        script1.src = 'https://www.gstatic.com/firebasejs/9.10.0/firebase-app.js';

        //const script2 = document.createElement('script');
        //script2.src = 'https://www.gstatic.com/firebasejs/9.10.0/firebase-auth.js';

        script1.onload = function() {
            // Firebase should now be globally available
            //const firebase = window.firebase; 

            // Your Firebase config (Caution: Security Considerations)
            const firebaseConfig = {
                apiKey: "AIzaSyDARPwQjBq0KveTeNHyDSz1p6YeGTHw_Do",
                authDomain: "test-openfort-extension.firebaseapp.com",
                projectId: "test-openfort-extension",
                storageBucket: "test-openfort-extension.appspot.com",
                messagingSenderId: "775020371354",
                appId: "1:775020371354:web:dfc54dfe47f020e184ae23"
            };      
            initializeApp(firebaseConfig)
                .then(() => {
                    Module.SendMessage("FirebaseAuthController", "OnFirebaseReady");
                })
                .catch((error) => {
                    Module.SendMessage("FirebaseAuthController", "OnFirebaseError", error.message);
                });
        };
        document.head.appendChild(script1);
    },

    signInWithGoogle: function(callbackFunctionNamePtr) { // Taking callback function name
        const provider = new firebase.auth.GoogleAuthProvider();
        firebase.auth().signInWithPopup(provider)
        .then((result) => {
            const idToken = result.credential.idToken;
            Module.SendMessage("FirebaseAuthController", "OnSignInSuccess", idToken);
            //let callbackFunctionName = Unity.Runtime.InteropServices.Marshal.PtrToStringAuto(callbackFunctionNamePtr);
            //Unity[callbackFunctionName](idToken); // Calling a Unity function
        })
        .catch((error) => {            
            // Similarly, handle errors and pass to Unity
            const errorMessage = error.message;
            Module.SendMessage("FirebaseAuthController", "OnSignInError", errorMessage);
        });
    }
});
*/