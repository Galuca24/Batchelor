import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@mui/material';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { Link } from 'react-router-dom';
const FineDetailsModal = ({ open, onClose, fine }) => {
  // Define images for different statuses
  const paidImageUrl = "/img/paidBook.PNG";  // Path to the image for paid fines
  const unpaidImageUrl = "/img/fineBook.PNG";  // Path to the image for unpaid fines

  // Ensure safe access to properties
  const safeFine = fine || {};
  const user = safeFine.user || {};

  // Determine which image to display
  const imageUrl = safeFine.isPaid ? paidImageUrl : unpaidImageUrl;

  const theme = createTheme({
    palette: {
      primary: {
        main: '#006400',
      },
    },
  });

  return (
    <ThemeProvider theme={theme}>
      <Dialog
        open={open}
        onClose={onClose}
        aria-labelledby="fine-details-title"
        fullWidth
        maxWidth="md"
        PaperProps={{
          sx: { minHeight: '60vh', maxWidth: '50vw', width: '100%', height: 'auto' }
        }}
      >
        <DialogTitle id="fine-details-title" className="bg-gray-50 text-lg font-semibold">
          Fine Details
        </DialogTitle>
        <DialogContent dividers className="bg-white p-4 flex flex-col lg:flex-row">
          <img
            src={imageUrl}  // Use the determined image URL based on fine payment status
            alt="Fine Details"
            className="w-full h-auto object-cover rounded-lg lg:w-5/12 xl:w-5/12 lg:mr-6"
          />
          <div className="flex-1 min-w-0 lg:mt-0 mt-6">
            <p className="text-xl mb-4 text-gray-800 font-medium">
              <strong>User Name:</strong>
              <Link to={`/dashboard/profile/${user.userId}`} className="ml-3 text-blue-600 hover:text-blue-800">
                {user.name}
              </Link>
            </p>            
            <p className="text-xl mb-4 text-gray-800 font-medium"><strong>User Email:</strong> {user.email}</p>
            <p className="text-xl mb-4 text-gray-800 font-medium"><strong>Issued Date:</strong> {new Date(safeFine.createdAt).toLocaleDateString()}</p>
            <p className="text-xl mb-4 text-gray-800 font-medium"><strong>Payment Date:</strong> {safeFine.isPaid ? new Date(safeFine.paidAt).toLocaleDateString() : 'Not Paid'}</p>
            <p className="text-xl mb-4 text-gray-800 font-medium"><strong>Amount:</strong> ${safeFine.amount}</p>
            <p className="text-xl mb-4 text-gray-800 font-medium">
              <strong>Status:</strong>
              <span className={`${safeFine.isPaid ? 'text-green-500' : 'text-red-500'}`}>
                {safeFine.isPaid ? 'Paid' : 'Unpaid'}
              </span>
            </p>
          </div>
        </DialogContent>
        <DialogActions className="bg-gray-50">
          <Button onClick={onClose} color="primary">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </ThemeProvider>
  );
};

export default FineDetailsModal;
