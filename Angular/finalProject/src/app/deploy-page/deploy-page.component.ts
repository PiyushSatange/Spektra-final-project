import { Component, OnInit } from '@angular/core';
import { FileUploadService } from '../services/file-upload.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-deploy-page',
  templateUrl: './deploy-page.component.html',
  styleUrl: './deploy-page.component.css'
})
export class DeployPageComponent implements OnInit{
  isDragOverAws = false;
  isDragOverGcp = false;
  isDragOverAzure = false;
  droppedFileAws: File | null = null;
  droppedFileGcp: File | null = null;
  droppedFileAzure: File | null = null;
  isloggedIn = false;
  counter = 0;
  private timerInterval: any;
  messages = [
    'Sending file to our server',
    'Reading the data from you file', 
    'Running Terraform init command...',
    'Running terraform auto apply command...',
    'Please wait, we are processing your request...',
    'This might take a while, hold tight...',
    'Still working on it, thank you for your patience...',
  ];
  messageIndex = 0;

  constructor(private fileService: FileUploadService){}

  ngOnInit(): void {
    if(localStorage.getItem('uid')!=null){
      this.isloggedIn = true;
    }
    else{
      this.isloggedIn = false;
    }
  }

  onFileSelectedAws(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.droppedFileAws = input.files[0];
      console.log('File selected:', this.droppedFileAws);
      Swal.fire({
        position: "center",
        icon: "success",
        title: "File uploaded successfully",
        showConfirmButton: false,
        timer: 2000
      });
      const fileName = this.droppedFileAws.name;
      const fileExtension = fileName.split('.').pop();
      this.checkValidfile(fileExtension);
    }
  }

  onFileSelectedGcp(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.droppedFileGcp = input.files[0];
      console.log('File selected:', this.droppedFileGcp);
      Swal.fire({
        position: "center",
        icon: "success",
        title: "File uploaded successfully",
        showConfirmButton: false,
        timer: 2000
      });
      const fileName = this.droppedFileGcp.name;
      const fileExtension = fileName.split('.').pop();
      this.checkValidfile(fileExtension);
    }
  }

  onFileSelectedAzure(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.droppedFileAzure = input.files[0];
      console.log('File selected:', this.droppedFileAzure);
      Swal.fire({
        position: "center",
        icon: "success",
        title: "File uploaded successfully",
        showConfirmButton: false,
        timer: 2000
      });
      const fileName = this.droppedFileAzure.name;
      const fileExtension = fileName.split('.').pop();
      this.checkValidfile(fileExtension);
    }
  }

  onDragOverAws(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverAws = true;
    console.log('Drag over'); // Log drag over event
  }

  onDragOverGcp(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverGcp = true;
    console.log('Drag over'); // Log drag over event
  }

  onDragOverAzure(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverAzure = true;
    console.log('Drag over'); // Log drag over event
  }
  onDragLeaveAws(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverAws = false;
    console.log('Drag leave'); // Log drag leave event
  }
  onDragLeaveGcp(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverGcp = false;
    console.log('Drag leave'); // Log drag leave event
  }
  onDragLeaveAzure(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverAzure = false;
    console.log('Drag leave'); // Log drag leave event
  }

  onDropAws(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverAws = false;
    console.log('Drop event'); // Log drop event
    if (event.dataTransfer && event.dataTransfer.files && event.dataTransfer.files.length > 0) {
      this.droppedFileAws = event.dataTransfer.files[0];
      const fileName = this.droppedFileAws.name;
      const fileExtension = fileName.split('.').pop();
      console.log('File dropped:', this.droppedFileAws); // Log the dropped file
      Swal.fire({
        position: "center",
        icon: "success",
        title: "File uploaded successfully",
        showConfirmButton: false,
        timer: 2000
      });
      this.checkValidfile(fileExtension);
    }
  }

  onDropGcp(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverGcp = false;
    console.log('Drop event'); // Log drop event
    if (event.dataTransfer && event.dataTransfer.files && event.dataTransfer.files.length > 0) {
      this.droppedFileGcp = event.dataTransfer.files[0];
      const fileName = this.droppedFileGcp.name;
      const fileExtension = fileName.split('.').pop();
      console.log('File dropped:', this.droppedFileGcp); // Log the dropped file
      Swal.fire({
        position: "center",
        icon: "success",
        title: "File uploaded successfully",
        showConfirmButton: false,
        timer: 2000
      });
      this.checkValidfile(fileExtension);
    }
  }

  onDropAzure(event: DragEvent): void {
    event.preventDefault();
    this.isDragOverAzure = false;
    console.log('Drop event'); // Log drop event
    if (event.dataTransfer && event.dataTransfer.files && event.dataTransfer.files.length > 0) {
      this.droppedFileAzure = event.dataTransfer.files[0];
      const fileName = this.droppedFileAzure.name;
      const fileExtension = fileName.split('.').pop();
      console.log('File dropped:', this.droppedFileAzure); // Log the dropped file
      Swal.fire({
        position: "center",
        icon: "success",
        title: "File uploaded successfully",
        showConfirmButton: false,
        timer: 2000
      });
      this.checkValidfile(fileExtension);
    }
  }

  checkValidfile(fileExtension:any){
    if (fileExtension == 'tf') {
      console.log('This is valid file'); // Log the selected file
    } else {
      console.error('Invalid file type. Please select a .tf file.');
    }
  }


  
    

  onAwsUpload() {
    if (this.droppedFileAws != null) {
      console.log(this.droppedFileAws.name);
      console.log("aws function Working...");
      Swal.fire({
        title: 'Loading...',
        text: 'Please wait',
        allowOutsideClick: false,
        allowEscapeKey: false,
        showConfirmButton: false,
        didOpen: () => {
          Swal.showLoading();
        }
      });
      
      this.fileService.uploadAwsFile(this.droppedFileAws, localStorage.getItem("email")).subscribe(
        (data) => {
          console.log('Upload successful:', data);
          //this.loading = false;
          Swal.fire({
            icon: 'success',
            title: 'Success!',
            text: 'Your operation was successful.',
            confirmButtonText: 'OK'
          });
        },
        (error) => {
          console.error('Upload failed:', error);
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Something went wrong!",
            footer: '<a href="#">Why do I have this issue?</a>'
          });
        }

      );
    } else {
      console.log("The file is not dropped");
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Something went wrong!",
        footer: '<a href="#">Why do I have this issue?</a>'
      });
    }
  }
  onAzureUpload() {
    if (this.droppedFileAzure != null) {
     Swal.fire({
      title: 'Loading...',
      text: 'Please wait',
      allowOutsideClick: false,
      allowEscapeKey: false,
      showConfirmButton: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });
    
      this.fileService.uploadAzureFile(this.droppedFileAzure, localStorage.getItem("email")).subscribe(
        (data) => {
          console.log('Upload successful:', data);
          Swal.fire({
            icon: 'success',
            title: 'Success!',
            text: 'Your operation was successful.',
            confirmButtonText: 'OK'
          });
        },
        (error) => {
          console.error('Upload failed:', error);
          Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Something went wrong!",
            footer: '<a href="#">Why do I have this issue?</a>'
          });
        }
      );
    } else {
      console.log("The file is not dropped");
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Something went wrong!",
        footer: '<a href="#">Why do I have this issue?</a>'
      });
    }
  }

  
}