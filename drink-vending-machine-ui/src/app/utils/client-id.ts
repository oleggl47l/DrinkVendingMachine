export const getOrCreateClientId = () => {
    if (typeof window === 'undefined') return null;

    let clientId = localStorage.getItem('clientId');
    if (!clientId) {
        clientId = crypto.randomUUID();
        localStorage.setItem('clientId', clientId);
    }
    return clientId;
};