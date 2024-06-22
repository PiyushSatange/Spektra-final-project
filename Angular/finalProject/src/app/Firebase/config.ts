// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
import {getAuth} from 'firebase/auth';
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyBXdxaILLbzAy1596fHd5tTj7mRd6WzLFo",
  authDomain: "spektra-final-project.firebaseapp.com",
  projectId: "spektra-final-project",
  storageBucket: "spektra-final-project.appspot.com",
  messagingSenderId: "57762745136",
  appId: "1:57762745136:web:2ae14819742d78c73d56f5",
  measurementId: "G-S5D93KN6LE"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
// const analytics = getAnalytics(app);

export const auth = getAuth(app);