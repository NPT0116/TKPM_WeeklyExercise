import React, { useState } from "react";

interface SearchBarProps {
  onSearch: (studentId: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ onSearch }) => {
  const [studentId, setStudentId] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    onSearch(studentId);
  };

  return (
    <form className="search-bar" onSubmit={handleSubmit}>
      <input
        type="text"
        value={studentId}
        onChange={(e) => setStudentId(e.target.value)}
        placeholder="Nhập MSSV..."
        className="search-input"
      />
      <button type="submit" className="search-button">
        Tìm kiếm
      </button>
    </form>
  );
};

export default SearchBar;
