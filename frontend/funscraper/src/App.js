import React, { useState, useEffect } from 'react';

function App() {
  const [selectedOption, setSelectedOption] = useState('');
  const [tableData, setTableData] = useState([]);

  useEffect(() => {
    if (selectedOption) {
      var url = selectedOption == 'vnexpress' ? `http://localhost:5277/vnexpress` : `http://localhost:5277/tuoitre`;
      console.log(url);
      fetch(url, { method: 'GET', mode: 'cors'})
        .then((response) => {
          if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
          }
          return response.json();
        })
        .then((data) => {
          console.log(data.data);
          setTableData(data.data);
    })
        .catch((error) => console.error('Error fetching data:', error));
    }
  }, [selectedOption]);

  const handleSelectChange = (e) => {
    setSelectedOption(e.target.value);
  };

  return (
    <div>
      <h1>Fun Web Scraper</h1>
      <select onChange={handleSelectChange} value={selectedOption}>
        <option value="">Select an option</option>
        <option value="vnexpress">vnexpress</option>
        <option value="tuoitre">tuoitre</option>
      </select>

      {tableData.length > 0 && (
        <table>
          <thead>
            <tr>
              <th>Like</th>
              <th>Title</th>
              <th>Url</th>
            </tr>
          </thead>
          <tbody>
            {tableData.map((row, index) => (
              <tr key={index}>
                <td>{row.like}</td>
                <td>{row.title}</td>
                <td><a href={row.url}>{row.url}</a></td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default App;
