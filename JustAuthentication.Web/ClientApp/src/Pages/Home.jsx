import React, {useState} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Home.css';
import { useAuthentication } from '../AuthContext';

const Home = () => {

    const { user } = useAuthentication();

    return (
        <div style={{ marginTop: 80 }}>
            {!user && <h1>Welcome, Guest!</h1>}
            {!!user && <h1>Hello, {user.name}</h1> }
        </div>
    );
};

export default Home;