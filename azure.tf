provider "azurerm" {
  features {}
  subscription_id = "c196a0b3-dde0-4c93-b661-551c2f55b6c6"
  client_id       = "b5f08f3c-a683-4763-8f67-faa7e8c62058"
  client_secret   = "Yc68Q~7QsIS7wKnjDSfbWKqm4P-lbZjJ6OfpBaK0"
  tenant_id       = "a6f9c9e8-8623-43f5-a543-74108fd93a01"
}

resource "azurerm_resource_group" "example" {
  name     = "omsatange"
  location = "West Europe"
}