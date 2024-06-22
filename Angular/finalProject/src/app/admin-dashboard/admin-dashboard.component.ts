import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Chart } from 'chart.js/auto';
import 'chartjs-adapter-date-fns'; // Import the date adapter
import * as d3 from 'd3';
@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  @ViewChild('myAwsChart', { static: true }) barChart!: ElementRef<HTMLCanvasElement>;
  @ViewChild('myAzureChart', { static: true }) lineChart!: ElementRef<HTMLCanvasElement>;
  @ViewChild('chart', {static: true})  chartContainer!: ElementRef<HTMLCanvasElement>;

  private awsData1 = {
    serviceCosts: {
      'EC2': 1200,
      'S3': 600,
      'Lambda': 300,
      'RDS': 800,
      'CloudFront': 500
    }
  };

  startDate: any;
  endDate: any;
  awsData: any;
  azureData: any;
  awschart: any;
  azurechart: any;
  TotalAzureCost: number = 0;
  TotalCloudCost : number = 0;
  admin: any | null;
  isLoggedIn = true;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.admin = localStorage.getItem("name");
    this.startDate = "2024-06-10";
    this.endDate = "2024-06-15";
    this.sendDates();
    this.createAwsChart();
    console.log("this is admit"+this.admin);
    if(this.admin === null){
      this.isLoggedIn = true;
    }
  }

  
  sendDates() {
    this.TotalCloudCost = 0;
    this.TotalAzureCost = 0;
    const formattedStartDate = new Date(this.startDate).toISOString();
    const formattedEndDate = new Date(this.endDate).toISOString();
    // console.log(this.startDate);
    // console.log(this.endDate);
    // console.log(formattedStartDate);
    // console.log(formattedEndDate);

    const dates = {
      startDate: formattedStartDate,
      endDate: formattedEndDate
    };

    this.http.get(`https://localhost:7072/api/Cost/GetAzureCostDateWise?from=${this.startDate}&to=${this.endDate}`)
      .subscribe(response => {
        console.log('Azure data received:', response);
        this.azureData = response;
        for (let i = 0; i < this.azureData.length; i++) {
          console.log("this is the azure data"+this.azureData[i].cost);
          this.TotalAzureCost += this.azureData[i].cost;
          console.log(this.TotalAzureCost+" this is the plus");
        }
        this.TotalCloudCost += (this.TotalAzureCost/82);
        if (this.azureData) {
          console.log("Preparing to create Azure chart");
          if(this.azurechart){
            this.azurechart.destroy();
          }
          this.createAzureChart();
        }
      }, error => {
        console.error('Error fetching Azure data', error);
      }
    );

    this.http.post('https://localhost:7072/api/AwsCost/calculateTotalCost', dates)
      .subscribe(response => {
        console.log('AWS data received:', response);
        this.awsData = response;
        this.TotalCloudCost += this.awsData.totalCost;
        if (this.awsData) {
          console.log("Preparing to create AWS chart");
          if(this.awschart){
            this.awschart.destroy();
          }
          this.createAwsChart();
        }
      }, error => {
        console.error('Error fetching AWS data', error);
      }
    );
  }

  createAzureChart() {
    const dates = this.azureData.map((item: any) => new Date(item.date).getDate());
    const costs = this.azureData.map((item: any) => item.cost);

    console.log('Azure Chart data - Dates:', dates);
    console.log('Azure Chart data - Costs:', costs);

    const ctx = this.lineChart.nativeElement.getContext('2d');

    if (ctx) {
      this.azurechart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: dates,
          datasets: [{
            label: 'Cost',
            data: costs,
            backgroundColor: [
              'rgba(44, 62, 80, 0.2)',   // Dark Blue
              'rgba(41, 128, 185, 0.2)', // Blue
              'rgba(39, 174, 96, 0.2)',  // Green
              'rgba(192, 57, 43, 0.2)',  // Red
              'rgba(142, 68, 173, 0.2)', // Purple
              'rgba(243, 156, 18, 0.2)'  // Orange
            ]
            ,
            borderColor: [
              'rgba(44, 62, 80, 1)',   // Dark Blue
              'rgba(41, 128, 185, 1)', // Blue
              'rgba(39, 174, 96, 1)',  // Green
              'rgba(192, 57, 43, 1)',  // Red
              'rgba(142, 68, 173, 1)', // Purple
              'rgba(243, 156, 18, 1)'  // Orange
            ]
            ,
            borderWidth: 2
          }]
        },
        options: {
          responsive: true,
          plugins: {
            legend: {
              display: true,
              position: 'top',
            },
            tooltip: {
              enabled: true,
              callbacks: {
                label: function(tooltipItem: any) {
                  return `${tooltipItem.label}: ${tooltipItem.raw}`;
                }
              }
            }
          },
        }
        
      }
    );
    console.log("chart created successfylly");
    } else {
      console.error('Failed to get 2D context for Azure chart');
    }
  }

  createAwsChart() {
    const labels = Object.keys(this.awsData.serviceCosts);
    const data = Object.values(this.awsData.serviceCosts);

    console.log("AWS Chart labels: ", labels);
    console.log("AWS Chart data: ", data);

    const ctx = this.barChart.nativeElement.getContext('2d');

    if (ctx) {
      this.awschart = new Chart(ctx, {
        type: 'bar',
        data: {
          labels: labels,
          datasets: [{
            label: 'Service Costs',
            data: data,
            backgroundColor: [
              'rgba(44, 62, 80, 0.2)',   // Dark Blue
              'rgba(41, 128, 185, 0.2)', // Blue
              'rgba(39, 174, 96, 0.2)',  // Green
              'rgba(192, 57, 43, 0.2)',  // Red
              'rgba(142, 68, 173, 0.2)', // Purple
              'rgba(243, 156, 18, 0.2)'  // Orange
            ]
            ,
            borderColor: [
              'rgba(44, 62, 80, 1)',   // Dark Blue
              'rgba(41, 128, 185, 1)', // Blue
              'rgba(39, 174, 96, 1)',  // Green
              'rgba(192, 57, 43, 1)',  // Red
              'rgba(142, 68, 173, 1)', // Purple
              'rgba(243, 156, 18, 1)'  // Orange
            ]
            ,
            borderWidth: 4
          }]
        },
        options: {
          scales: {
            y: {
              beginAtZero: true
            }
          }
        }
      });
      console.log("AWS chart created successfully");
    } else {
      console.error('Failed to get 2D context for AWS chart');
    }
  }
}