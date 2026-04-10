import { useEffect, useState } from 'react';
import Auth from './Components/Auth';
import './App.css';

function App() {
    const [products, setProducts] = useState();
    const [token, setToken] = useState(localStorage.getItem('token'));
    const [colourFilter, setColourFilter] = useState('');
    const [debouncedColourFilter, setDebouncedColourFilter] = useState('');
    const [isAdding, setIsAdding] = useState(false);
    const [error, setError] = useState(null);

    const handleLogout = () => {
        localStorage.removeItem('token');
        setToken(null);
        setProducts(undefined);
        setError(null);
    };

    // Debounce colour filter
    useEffect(() => {
        const handler = setTimeout(() => {
            setDebouncedColourFilter(colourFilter);
        }, 500);
        return () => clearTimeout(handler);
    }, [colourFilter]);

    const handleAddProduct = async (e) => {
        e.preventDefault();
        setError(null);
        setIsAdding(true);

        const form = e.target;
        const newProduct = {
            productName: form.productName.value,
            colour: form.colour.value,
            productPrice: parseFloat(form.productPrice.value)
        };

        try {
            const response = await fetch('https://localhost:7266/api/Products/AddProduct', {
                method: 'POST',
                headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' },
                body: JSON.stringify(newProduct)
            });
            if (response.ok) {
                const addedProduct = await response.json();
                setProducts([...products, addedProduct]);
                form.reset(); // Clear form on success
            }
            else if (response.status === 401) {
                handleLogout();
            } else {
                setError('Failed to add product. Please check your inputs.');
            }
        }
        catch (error) {
            console.error('Error adding product:', error);
            setError('A network error occurred while adding the product.');
        }
        finally { setIsAdding(false); }
    };

    useEffect(() => {
        if (!token) return;

        const fetchProducts = async () => {
            setError(null);
            try {
                const response = await fetch(`https://localhost:7266/api/Products/GetProducts?colour=${debouncedColourFilter}`, {
                    method: 'GET',
                    headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' }
                });
                if (response.ok) {
                    setProducts(await response.json());
                }
                else if (response.status === 401) {
                    handleLogout();
                } else {
                    setError('Failed to load products.');
                }
            } catch  {
                setError('Could not connect to the server.');
            }
        };

        fetchProducts();
    }, [token, debouncedColourFilter]);

    const contents = token === null ?
        <Auth onLoginSuccess={(newToken) => setToken(newToken)} />
        :
        products === undefined
            ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
            :
            <div style={{ padding: '20px' }}>

                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <h1 id="tableLabel">Products</h1>
                    <button onClick={handleLogout} style={{ padding: '8px 16px', cursor: 'pointer' }}>Logout</button>
                </div>

                {error && (
                    <div style={{ color: 'red', padding: '10px', backgroundColor: '#fee', borderRadius: '4px', marginBottom: '20px' }}>
                        {error}
                    </div>
                )}

                <div>
                    <form onSubmit={handleAddProduct}>
                        <div style={{ margin: 20 }}>
                            <label style={{ fontWeight: 'bold', display: 'block', marginBottom: '10px' }}>Add New Product</label>
                            <input style={{ margin: 2, padding: '8px' }} type="text" name="productName" placeholder="Product Name" required />
                            <input style={{ margin: 2, padding: '8px' }} type="text" name="colour" placeholder="Colour" required />
                            <input style={{ margin: 2, padding: '8px' }} type="number" name="productPrice" placeholder="Price" step="0.01" required />
                            <button style={{ margin: 2, padding: '8px 16px', cursor: 'pointer' }} type="submit" disabled={isAdding}>
                                {isAdding ? 'Adding...' : 'Add Product'}
                            </button>
                        </div>
                    </form>
                </div>
                <div style={{ marginBottom: 20 }}>
                    <label htmlFor="colourFilter" style={{ marginRight: '10px' }}>Filter by Colour:</label>
                    <input
                        id="colourFilter"
                        type="text"
                        value={colourFilter}
                        onChange={(e) => setColourFilter(e.target.value)}
                        placeholder="Type to filter..."
                        style={{ padding: '8px', width: '200px' }}
                    />
                </div>
                <div style={{ display: 'flex', justifyContent: 'center' }}>
                    <table className="table table-striped" aria-labelledby="tableLabel" style={{ width: '80%', maxWidth: '900px', margin: '0 auto', borderCollapse: 'collapse' }}>
                        <thead>
                            <tr style={{ backgroundColor: '#f2f2f2' }}>
                                <th style={{ padding: '12px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>ID</th>
                                <th style={{ padding: '12px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>Name</th>
                                <th style={{ padding: '12px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>Colour</th>
                                <th style={{ padding: '12px', textAlign: 'left', borderBottom: '2px solid #ddd' }}>Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            {products.length > 0 ? products.map(product =>
                                <tr key={product.id}>
                                    <td style={{ padding: '12px', textAlign: 'left', borderBottom: '1px solid #ddd' }}>{product.id}</td>
                                    <td style={{ padding: '12px', textAlign: 'left', borderBottom: '1px solid #ddd' }}>{product.productName}</td>
                                    <td style={{ padding: '12px', textAlign: 'left', borderBottom: '1px solid #ddd' }}>{product.colour}</td>
                                    <td style={{ padding: '12px', textAlign: 'left', borderBottom: '1px solid #ddd' }}>${product.productPrice.toFixed(2)}</td>
                                </tr>
                            ) : (
                                <tr>
                                    <td colSpan="4" style={{ padding: '20px', textAlign: 'center' }}>No products found matching that colour.</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
            </div>;
    return (
        <div style={{ padding: '20px' }}>
            {contents}
        </div>
    );
}

export default App;