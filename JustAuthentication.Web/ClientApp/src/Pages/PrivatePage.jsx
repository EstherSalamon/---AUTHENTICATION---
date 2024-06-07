import { useEffect, useState } from 'react';
import axios from 'axios';

const PrivatePage = () => {

    const [message, setMessage] = useState('');

    useEffect(() => {

        const getData = async () => {
            const { data } = await axios.get('/api/Users/privatedata');
            setMessage(data);
        }

        getData();

    }, []);

    return (
        <div style={{ marginTop: 80 }}>
            <h1>{message}</h1>
        </div>
    )

}

export default PrivatePage;