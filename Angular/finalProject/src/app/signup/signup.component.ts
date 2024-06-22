import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { createUserWithEmailAndPassword } from 'firebase/auth';
import { auth } from '../Firebase/config';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent {
  signupForm! : FormGroup;

  constructor(private fb:FormBuilder, private http:HttpClient, private router:Router){}

  ngOnInit(): void {
    this.signupForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email,  Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,4}$')]],
      password: ['', Validators.required],
      agree: [false, Validators.requiredTrue]
    })
  }

  _signUp(){
    console.log(this.signupForm.value);
    createUserWithEmailAndPassword(auth, this.signupForm.value.email, this.signupForm.value.password)
    .then((UserCrediantial) => {
      Swal.fire({
        title: "Signed Up!",
        text: "Your accound has been created!",
        icon: "success"
      });
        console.log(this.signupForm.value.name+" "+UserCrediantial.user.email+" "+UserCrediantial.user.uid);
        const userData = {
          email: UserCrediantial.user.email,
          name: this.signupForm.value.name,
          uid: UserCrediantial.user.uid
        };
        this.http.post('https://localhost:7072/api/User/adduser', userData)
        .subscribe(
          response => {
            console.log('User data posted successfully', userData);
            this.router.navigate(['/login']);
          },
          error => {
            console.error('Error posting user data', error);
            Swal.fire({
              icon: "error",
              title: "Oops...",
              text: "Something went wrong!",
              footer: '<a href="#">Why do I have this issue?</a>'
            });
          }
        );
    })
    .catch((error) => {
      console.error('error creating user');
      console.error('Error posting user data', error);
            Swal.fire({
              icon: "error",
              title: "Oops...",
              text: "User already exist!",
              footer: '<a href="#">Why do I have this issue?</a>'
            });
    })
  }
}
