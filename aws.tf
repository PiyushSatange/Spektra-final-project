provider "aws" {
  region                  = "us-west-1" 
  access_key              = "AKIAS25ZBBV6M6QXZE5K"
  secret_key              = "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"
}

resource "aws_s3_bucket" "my_bucket" {
  bucket = "unique-bucket-shibu"
  acl    = "private" 
}
