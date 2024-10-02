import React, { useState, useEffect } from 'react';
import { Card, CardContent, Typography, Avatar, Button, TextareaAutosize } from '@mui/material';
import { TrashIcon } from '@heroicons/react/24/solid';
import { useUser } from '../../../../context/LoginRequired';
import { toast } from 'react-toastify';
import api from '@/services/api';

const AudioBookReviewSection = ({ audioBookId }) => {
    const currentUser = useUser();
    const [reviews, setReviews] = useState([]);
    const [newReviewText, setNewReviewText] = useState('');
    const [focusStyle, setFocusStyle] = useState({});

    const s3BaseUrl = "https://galabucket.s3.eu-north-1.amazonaws.com/";

    const handleFocus = () => {
        setFocusStyle({
            border: '1px transparent',
            height: '50px',
            width: '100%',
            outline: 'none' 
        });
    };

    const handleBlur = () => {
        setFocusStyle({
            height: '50px',
            width: '100%',
        });
    };

    useEffect(() => {
        fetchReviews();
    }, [audioBookId]);

    const fetchReviews = async () => {
        try {
            const response = await api.get(`/api/v1/review/getreviewsbyaudiobookid?audioBookId=${audioBookId}`, {
                headers: {
                    Authorization: `Bearer ${currentUser.token}`,
                },
            });

            if (response.status === 200) {
                const reversedReviews = response.data.reviews.reverse();
                setReviews(reversedReviews);
            } else {
                console.error('Error fetching reviews:', response);
                toast.error('Error fetching reviews');
            }
        } catch (error) {
            console.error('Error fetching reviews:', error);
            toast.error('Failed to fetch reviews');
        }
    };

    const handleAddReview = async () => {
        if (newReviewText.trim() === "") {
            toast.error("Review text cannot be empty.");
            return;
        }
        try {
            const response = await api.post('/api/v1/review/create-review-for-audio-book', {
                audioBookId: audioBookId,
                userId: currentUser.userId,
                reviewText: newReviewText,
                createdAt: new Date().toISOString(),
            }, {
                headers: {
                    Authorization: `Bearer ${currentUser.token}`,
                },
            });

            if (response.status === 200) {
                setNewReviewText('');
                toast.success("Review added successfully!");
                fetchReviews();
            } else {
                console.error('Error adding review:', response);
                toast.error('Failed to add review');
            }
        } catch (error) {
            console.error('Error adding review:', error);
            toast.error('Failed to add review');
        }
    };

    const handleDeleteReview = async (reviewId) => {
        try {
            const response = await api.delete(`/api/v1/review/deletereview`, {
                headers: {
                    Authorization: `Bearer ${currentUser.token}`,
                },
                data: { reviewId: reviewId }
            });
            if (response.status === 200) {
                toast.success('Review deleted successfully');
                fetchReviews();
            } else {
                console.error('Error deleting review:', response);
                toast.error('Failed to delete review');
            }
        } catch (error) {
            console.error('Error deleting review:', error);
            toast.error('Failed to delete review');
        }
    };

    return (
        <div className='flex flex-col mt-1'>
            <Typography variant="h6" gutterBottom>
                Reviews
            </Typography>
            <div className="overflow-auto xl:max-h-[7rem] lg:max-h-[7rem] max-h-[14rem]" style={{ scrollbarWidth: 'thin' }}>
                {reviews.map((review) => (
                    <Card key={review.reviewId} className="mb-2">
                        <CardContent className='p-2'>
                            <div className='flex items-center'>
                                <Avatar
                                    src={`${s3BaseUrl}${review.createdBy.userPhoto?.photoUrl}`}
                                    alt={review.createdBy.name}
                                />
                                <div className='flex-grow ml-2'>
                                    <Typography variant="subtitle1">{review.createdBy.name}</Typography>
                                    <Typography variant="body2" color="text.secondary">{review.reviewText}</Typography>
                                    <Typography variant="caption" color="text.secondary">{new Date(review.datePosted).toLocaleString()}</Typography>
                                </div>
                                {review.createdBy.userId === currentUser.userId && (
                                    <div className="group relative">
                                        <TrashIcon
                                            onClick={() => handleDeleteReview(review.reviewId)}
                                            className="h-5 w-5 text-red-500 cursor-pointer"
                                        />
                                    </div>
                                )}
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>
            <textarea
                placeholder="Write a review"
                value={newReviewText}
                onChange={(e) => setNewReviewText(e.target.value)}
                onKeyDown={(e) => {
                    if (e.key === 'Enter' && !e.shiftKey) {
                        e.preventDefault();
                        handleAddReview();
                    }
                }}
                onFocus={handleFocus}
                onBlur={handleBlur}
                style={focusStyle}
            />
        </div>
    );
};

export default AudioBookReviewSection;
