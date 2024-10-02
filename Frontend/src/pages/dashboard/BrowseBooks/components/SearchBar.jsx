import React, { useState } from 'react';
import { Input, Button } from "@material-tailwind/react";

const SearchBar = ({ onSearch }) => {
    const [searchTerm, setSearchTerm] = useState('');

    const handleSearch = () => {
        onSearch(searchTerm);
    };

    return (
        <div className="flex items-center space-x-2">
            <Input
                type="text"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                placeholder="Search books..."
                color="lightBlue"
            />
            <Button onClick={handleSearch} color="lightBlue" size="regular">
                Search
            </Button>
        </div>
    );
};

export default SearchBar;