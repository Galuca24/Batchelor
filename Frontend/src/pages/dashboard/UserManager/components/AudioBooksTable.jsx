import React, { useState } from 'react';
import { TrashIcon, EyeIcon, PencilIcon } from "@heroicons/react/24/solid";
import DeleteBookConfirmationDialog from "./DeleteBookConfirmationDialog";
import axios from 'axios';
import { toast } from 'react-toastify';
import EditAudioBookModal from "./EditAudioBookModal";

const AudioBooksTable = ({ audioBooks, onDelete, setAudioBooks }) => {
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
  const [selectedAudioBook, setSelectedAudioBook] = useState(null);
  const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);
  const [isEditModalOpen, setIsEditModalOpen] = useState(false);

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10; // Poți ajusta acest număr după preferințe

  const changePage = (newPage) => {
    setCurrentPage(newPage);
  };

  const currentAudioBooks = audioBooks.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const pageCount = Math.ceil(audioBooks.length / itemsPerPage);
  const handleViewAudioBook = (audioBook) => {
    setSelectedAudioBook(audioBook);
    setIsDetailsModalOpen(true);
  };

  const handleOpenDeleteDialog = (audioBook) => {
    setSelectedAudioBook(audioBook);
    setOpenDeleteDialog(true);
  };

  const handleEdit = (audioBook) => {
    setSelectedAudioBook(audioBook);
    setIsEditModalOpen(true);
  };

  const handleBookUpdated = (updatedBook) => {
    const updatedBooks = audioBooks.map(audioBook => audioBook.audioBookId === updatedBook.audioBookId ? updatedBook : audioBook);
    setAudioBooks(updatedBooks);

    setIsEditModalOpen(false);
  };

  const handleCloseDeleteDialog = () => {
    setOpenDeleteDialog(false);
  };


  
  const handleConfirmDelete = async () => {
    if (!selectedAudioBook) return;

    try {
      const response = await axios.delete('https://localhost:7115/api/v1/audiobooks/delete-audio-book', {
        data: { audioBookId: selectedAudioBook.audioBookId }
      });
      if (response.data.success) {
        toast.success(response.data.message || "Audio Book has been deleted successfully.");
        onDelete(selectedAudioBook.audioBookId);
      } else {
        toast.error("Failed to delete the audio book");
      }
    } catch (error) {
      console.error('Error deleting audio book:', error);
      toast.error("Error occurred while deleting the audio book");
    }
    setOpenDeleteDialog(false);
  };

  function truncateString(str, num) {
    if (str.length > num) {
      return str.slice(0, num) + "...";
    } else {
      return str;
    }
  }

  return (
    <div className="overflow-x-auto relative shadow-md sm:rounded-lg bg-surface-white">
      <div className="flex justify-between items-center">
        <h2 className="text-base ml-5 mt-5 text-surface-black font-semibold leading-tight">Audio Books Inventory</h2>
      </div>
      <table className="w-full text-sm text-left text-gray-500 mt-5">
        <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
          <tr>
            <th scope="col" className="py-3 px-6">Audio Book Title</th>
            <th scope="col" className="py-3 px-6">Author</th>
            <th scope="col" className="py-3 px-6">ISBN</th>
            <th scope="col" className="py-3 px-6">Genre</th>
            <th scope="col" className="py-3 px-6">Duration</th>
            <th scope="col" className="py-3 px-6">Actions</th>
          </tr>
        </thead>
        <tbody>
        {currentAudioBooks.map((audioBook) => (
            <tr key={audioBook.audioBookId} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
              <td className="py-4 px-6 flex items-center space-x-3">
                <img src={audioBook.imageUrl} alt={audioBook.title} style={{ width: '50px', height: '50px', objectFit: 'cover' }} />
                <span>{truncateString(audioBook.title, 50)}</span>
              </td>
              <td className="py-4 px-6">{truncateString(audioBook.author, 50)}</td>
              <td className="py-4 px-6">{audioBook.isbn}</td>
              <td className="py-4 px-6">{audioBook.genre}</td>
              <td className="py-4 px-6">{audioBook.duration}</td>
              <td className="py-4 px-6">
                <div className="flex items-center space-x-4">
                  <div className="group relative">
                    <PencilIcon onClick={() => handleEdit(audioBook)} className="h-5 w-5 text-blue-500 cursor-pointer" />
                    <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                      Edit Book
                    </span>
                  </div>
                  <div className="group relative">
                    <TrashIcon onClick={() => handleOpenDeleteDialog(audioBook)} className="h-5 w-5 text-red-500 cursor-pointer" />
                    <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                      Remove Audio Book
                    </span>
                  </div>
                  <div className="group relative">
                    <EyeIcon onClick={() => handleViewAudioBook(audioBook)} className="h-5 w-5 text-blue-500 cursor-pointer" />
                    <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                      View Audio Book
                    </span>
                  </div>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {pageCount > 1 && (  // Afișează butoanele de paginare doar dacă există mai mult de o pagină
            <div className="flex justify-center p-4 text-black">
              {[...Array(pageCount).keys()].map(n => (
                <button
                  key={n + 1}
                  onClick={() => changePage(n + 1)}
                  className={`px-3 py-1 ${currentPage === n + 1 ? 'text-white bg-surface-dark-green' : 'bg-white'}`}>
                  {n + 1}
                </button>
              ))}
            </div>
          )}
      {selectedAudioBook && (
        <DeleteBookConfirmationDialog
          open={openDeleteDialog}
          onClose={handleCloseDeleteDialog}
          onConfirm={handleConfirmDelete}
          book={selectedAudioBook}
        />
      )}
      {selectedAudioBook && (
        <EditAudioBookModal
          open={isEditModalOpen}
          onClose={() => setIsEditModalOpen(false)}
          audioBook={selectedAudioBook}
          onAudioBookUpdated={handleBookUpdated} />
      )}
    </div>
  );
};

export default AudioBooksTable;
