provider "google" {
  credentials = file("path/to/service-account-key.json")
  project     = "your-project-id"
  region      = "us-central1"
}

resource "google_project" "my_project" {
  name       = "my-project"
  project_id = "my-project-id"
}

resource "google_project_service" "storage" {
  project = google_project.my_project.project_id
  service = "storage.googleapis.com"
}

resource "google_storage_bucket" "my_bucket" {
  name     = "unique-bucket-hgfvghjm" 
  location = "us-central1"
  project  = google_project.my_project.project_id
}
