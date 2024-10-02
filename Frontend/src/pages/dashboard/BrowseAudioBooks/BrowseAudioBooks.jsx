import React, { useState, useEffect } from 'react';
import axios from 'axios';
import AudioBookCard from "./components/AudioBookCard";
import AudioBookDetailsModal from './components/AudioBookDetailsModal';
import { MagnifyingGlassIcon } from '@heroicons/react/24/outline';
import { useUser } from '../../../context/LoginRequired';

const BrowseAudioBooks = ({ userId }) => {
    const [audioBooks, setAudioBooks] = useState([]);
    const [audioBookSearchTerm, setAudioBookSearchTerm] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [selectedAudioBook, setSelectedAudioBook] = useState(null);
    const [modalOpen, setModalOpen] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(8);
    const [hasMembership, setHasMembership] = useState(false);
    const user = useUser();

    useEffect(() => {
        const checkMembership = async () => {
            try {
                const response = await axios.get(`https://localhost:7115/api/v1/memberships/getmembershipstatusbyuserid?UserId=${user.userId}`);
                setHasMembership(response.data === 1);
            } catch (error) {
                console.error('Error checking membership:', error);
                setHasMembership(false);
            }
        };

        checkMembership();
    }, [user.userId]);

    const pageCount = Math.ceil(audioBooks.length / itemsPerPage);

    const changePage = (newPage) => {
        setCurrentPage(newPage);
    };

    const currentBooks = audioBooks.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    useEffect(() => {
        const fetchAudioBooks = async () => {
            setLoading(true);
            let audioBooksUrl = audioBookSearchTerm.trim()
                ? `https://localhost:7115/api/v1/audiobooks/search-audio-books?query=${encodeURIComponent(audioBookSearchTerm)}`
                : 'https://localhost:7115/api/v1/audiobooks/get-all-audiobooks';

            try {
                const response = await axios.get(audioBooksUrl);
                setAudioBooks(response.data.audioBooks || response.data);
                setLoading(false);
            } catch (err) {
                setError('Failed to fetch audio books');
                setLoading(false);
                console.error(err);
            }
        };

        const delayDebounceFn = setTimeout(() => {
            fetchAudioBooks();
        }, 500);

        return () => clearTimeout(delayDebounceFn);
    }, [audioBookSearchTerm]);

    useEffect(() => {
        const fetchAllAudioBooks = async () => {
            setLoading(true);
            try {
                const response = await axios.get('https://localhost:7115/api/v1/audiobooks/get-all-audiobooks');
                setAudioBooks(response.data.audioBooks || response.data);
                setLoading(false);
            } catch (err) {
                setError('Failed to fetch audio books');
                setLoading(false);
                console.error(err);
            }
        };

        fetchAllAudioBooks();
    }, []);

    if (loading) return <p>Loading audio books...</p>;
    if (error) return <p>Error loading audio books: {error}</p>;

    const handleOpenModal = (audioBook) => {
        setSelectedAudioBook(audioBook);
        setModalOpen(true);
    };

    const handleCloseModal = () => {
        setModalOpen(false);
    };

    const handlePurchaseMembership = async () => {
        try {
            const response = await axios.post('https://localhost:7115/api/payment/PurchaseMembership', { userId: user.userId });
            window.location.href = response.data.url;
        } catch (error) {
            console.error('Error purchasing membership:', error);
        }
    };

    return (
        <div className="mt-12 px-4 relative">
            {!hasMembership && (
                <div className="absolute inset-0 bg-gray-900 bg-opacity-50 flex flex-col justify-center items-center text-white z-10">
                    <h2 className="text-xl font-bold">You need a membership to access these audiobooks</h2>
                    <p className="mt-2">Please purchase a subscription to unlock all content</p>
                    <button onClick={handlePurchaseMembership} className="mt-4 px-4 py-2 bg-surface-dark-green hover:bg-surface-light-green rounded">
                        Purchase Membership
                    </button>
                </div>
            )}
            <div className={`${!hasMembership ? 'blur-sm' : ''}`}>
                <h1 className="text-2xl font-bold mb-4">Available Audio Books</h1>
                <div className="relative w-full sm:w-1/3 mb-3">
                    <div className="absolute inset-y-0 left-0 flex items-center pl-3">
                        <MagnifyingGlassIcon className="w-4 h-4 text-black dark:text-black" />
                    </div>
                    <input
                        type="search"
                        className="block w-full p-4 pl-10 text-sm xl:w-96 md:w-96 sm:w-96 rounded-lg bg-gray-50 focus:ring-surface-dark-green focus:border-surface-dark-green focus:outline-surface-dark-green dark:text-white"
                        placeholder="Search Audio Books"
                        autoComplete="off"
                        value={audioBookSearchTerm}
                        onChange={(e) => setAudioBookSearchTerm(e.target.value)}
                    />
                </div>
                <div className="grid grid-cols-1 lg:grid-cols-4 md:grid-cols-3 sm:grid-cols-2 gap-4 mt-10">
                    {currentBooks.map((audioBook, index) => (
                        <AudioBookCard
                            key={audioBook.audioBookId}
                            audioBook={audioBook}
                            onViewDetails={() => handleOpenModal(audioBook)}
                        />
                    ))}
                </div>
            </div>
            {selectedAudioBook && (
                <AudioBookDetailsModal
                    open={modalOpen}
                    onClose={handleCloseModal}
                    audioBook={selectedAudioBook}
                />
            )}
            <div className="flex justify-center p-4 text-black xl:mr-12">
                {[...Array(pageCount).keys()].map(n => (
                    <button
                        key={n + 1}
                        onClick={() => changePage(n + 1)}
                        className={`px-3 py-1 ${currentPage === n + 1 ? 'text-white bg-surface-dark-green' : 'bg-white'}`}>
                        {n + 1}
                    </button>
                ))}
            </div>
        </div>
    );
};

export default BrowseAudioBooks;
