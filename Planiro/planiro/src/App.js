import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Auth from './Components/Auth';
import DevTeamPage from './Components/DevTeamPage';
import TeamCodeDisplay from './Components/TeamCodeDisplay';
import TeamBoard from './Components/TeamBoard';
import { useState, useEffect } from 'react';

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const authStatus = localStorage.getItem('isAuthenticated');
        setIsAuthenticated(!!authStatus);
    }, []);

    return (
        <Router>
            <Routes>
                <Route
                    path="/auth"
                    element={<Auth setIsAuthenticated={setIsAuthenticated} />}
                />
                <Route
                    path="/dev-team"
                    element={
                        isAuthenticated ? (
                            <DevTeamPage />
                        ) : (
                            <Navigate to="/auth" />
                        )
                    }
                />
                <Route
                    path="/team-code"
                    element={
                        isAuthenticated ? (
                            <TeamCodeDisplay />
                        ) : (
                            <Navigate to="/auth" />
                        )
                    }
                />
                <Route
                    path="/*"
                    element={<Navigate to={isAuthenticated ? "/dev-team" : "/auth"} />}
                />
                // В App.js замените проблемную строку:
                <Route
                    path="/team-board"
                    element={
                        isAuthenticated ? (
                            <TeamBoard isTeamLead={localStorage.getItem('isTeamLead') === 'true'} />
                        ) : (
                            <Navigate to="/auth" />
                        )
                    }
                />
            </Routes>
        </Router>
    );
}

export default App;