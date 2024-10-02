import React from 'react';
import { createTheme, ThemeProvider } from '@mui/material/styles';

const theme = createTheme({
    palette: {
      primary: {
        main: '#006400', // un verde întunecat
      },
    },
  });

const BookCardForHomePage = ({ book }) => {
    return (
        <ThemeProvider theme={theme}>
        <div className="xl:w-44 p-2 rounded-lg shadow-sm bg-transparent flex flex-col items-center text-center hover:bg-transparent hover:shadow-lg transition duration-300 ease-in-out hover:border hover:border-gray-300">
            <img
                src={book.imageUrl || '/path/to/default/image.jpg'}
                alt={book.title}
                className="h-56 w-full object-cover rounded-lg mb-1" // Înălțimea redusă și lățimea ajustată
            />
            <div className="mt-auto w-full text-left">  
                <h3 className="text-sm font-semibold truncate text-black">{book.title}</h3> 
            </div>
        </div>
        </ThemeProvider>
    );
};

export default BookCardForHomePage;
