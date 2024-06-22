import { Component } from '@angular/core';
import { FormBuilder, FormGroup , Validators} from '@angular/forms';
import { createUserWithEmailAndPassword, signInWithCredential, signInWithEmailAndPassword } from 'firebase/auth';
import { auth } from '../Firebase/config';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;
  userData : any;
  isadmin = true;
  adminData : any;

  constructor(private fb: FormBuilder, private router: Router, private http:HttpClient) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      userEmail: ['', [Validators.required, Validators.email, Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
      userPassword: ['', Validators.required]
    });
  }

  _login() {
    console.log("working");
    console.log(this.loginForm.value)

    this.http.get(`https://localhost:7072/api/Admin/${this.loginForm.value.userEmail}/${this.loginForm.value.userPassword}`)
    .subscribe(data => {
      this.adminData = data;
      console.log(this.adminData);
      

      console.log(this.isadmin);

      // Check if admin exists, if yes, do not proceed with user sign-in
      if (data) {
        // Admin exists, handle admin login logic here
        // For example, show a message saying "Admin login successful"
        localStorage.clear();
        localStorage.setItem("name","admin");
        console.log("Admin login successful");
        this.router.navigate(['/Admin']);
        Swal.fire({
          title: "Logged In!",
          text: "You are logged in as a Admin!",
          icon: "success"
        });
      } else {
        // Admin does not exist, proceed with user sign-in
        signInWithEmailAndPassword(auth, this.loginForm.value.userEmail, this.loginForm.value.userPassword)
          .then(res => {
            console.log("sign in, ",res.user);
            localStorage.setItem("uid", res.user.uid);
            this.http.get(`https://localhost:7072/api/User/getByUid?uid=${res.user.uid}`).subscribe((data) => {
              this.userData = data;
              localStorage.setItem("name", this.userData[0].name);
              localStorage.setItem("email", this.userData[0].email);
              localStorage.setItem("uid", this.userData[0].uid);
              Swal.fire({
                title: "Success!",
                text: "You are Logged In!",
                icon: "success"
              });
            })
            this.router.navigate(['/Home']);
          })
          .catch((error) => {
            console.log(error);
            Swal.fire({
              icon: "error",
              title: "Oops...",
              text: "Credentials are not correct!",
              footer: '<a href="#">Why do I have this issue?</a>'
            });
          }
        );
      }
    });
  }
}