import React, { useState } from 'react';
import LanguagePieChart from './LanguagesPieChart';
import './App.css';

const GitHubUserSearch = () => {
  const [username, setUsername] = useState('');
  const [userData, setUserData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSearch = async () => {
    if (!username) return;

    setLoading(true);
    setError(null);
    setUserData(null);

    try {
      const response = await fetch("https://localhost:7001/api/GitHub/GetUser", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username }),
      });

      if (!response.ok) {
        throw new Error("Erro ao buscar dados do usuário");
      }

      const data = await response.json();
      setUserData(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="github-user-search">
      <h1>Buscar Usuário GitHub</h1>
      <input
        type="text"
        className="search-input"
        placeholder="Digite o nome do usuário do GitHub"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
      <button className="search-button" onClick={handleSearch}>
        Buscar
      </button>

      {loading && <div className="loading">Carregando...</div>}
      {error && <div className="error">{error}</div>}

      {userData && (
        <div className="user-profile">
          <h2>{userData.name}</h2>
          <img
            src={userData.avatar_url}
            alt={userData.name}
            className="avatar"
          />
          <p>
            <a
              href={userData.html_url}
              target="_blank"
              rel="noopener noreferrer"
              className="profile-link"
            >
              Ver perfil no GitHub
            </a>
          </p>

          <h3>Grafico de uso de linguagens:</h3>
          <LanguagePieChart languages={userData.languagesSom}/>

          <h3>Repositórios:</h3>
          <ul className="repositories-list">
            {userData.repositories.map((repo) => (
              <li key={repo.name}>
                <a
                  href={repo.html_url}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="repo-link"
                >
                  {repo.name}
                </a>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

export default GitHubUserSearch;
