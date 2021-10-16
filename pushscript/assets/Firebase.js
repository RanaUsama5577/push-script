// Your web app's Firebase configuration
var firebaseConfig = {
    apiKey: "AIzaSyB955p6-Z-jtnHVHoprLhQYiyjjLlwE8fs",
    authDomain: "usama-push.firebaseapp.com",
    projectId: "usama-push",
    storageBucket: "usama-push.appspot.com",
    messagingSenderId: "725224502046",
    appId: "1:725224502046:web:0ddcc0cb7ae5ef28fbcf0f",
    measurementId: "G-9B6YH11E8W"
};
firebase.initializeApp(firebaseConfig);
const db = firebase.firestore();
const storageRef = firebase.storage();

