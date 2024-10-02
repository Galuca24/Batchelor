import axios from 'axios';
import AWS from 'aws-sdk';

// Create an Axios instance for API calls to your backend
const instance = axios.create({
  baseURL: 'https://localhost:7115/',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Ensure environment variables are available
if (!import.meta.env.VITE_AWSAccessKey || !import.meta.env.VITE_AWSSecretKey) {
  console.error('AWS credentials are not set in environment variables');
}

// Configure AWS globally
AWS.config.update({
  region: 'eu-north-1',
  credentials: new AWS.Credentials(
    import.meta.env.VITE_AWSAccessKey,
    import.meta.env.VITE_AWSSecretKey
  )
});

// Create an S3 instance specifically configured for your bucket
export const s3 = new AWS.S3({
  params: { Bucket: 'galabucket' },
});

export const s3BaseUrl = "https://galabucket.s3.eu-north-1.amazonaws.com/";

export default instance;
