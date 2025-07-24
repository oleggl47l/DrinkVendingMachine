import {useState, useCallback} from "react";

interface Toast {
    id: number;
    message: string;
    type: 'success' | 'error';
    leaving?: boolean;
}

export const useToastManager = () => {
    const [toasts, setToasts] = useState<Toast[]>([]);

    const showToast = useCallback((message: string, type: 'success' | 'error') => {
        const id = Date.now();
        setToasts(prev => [...prev, {id, message, type}]);

        setTimeout(() => {
            setToasts(prev =>
                prev.map(t => (t.id === id ? {...t, leaving: true} : t))
            );
        }, 3600);

        setTimeout(() => {
            setToasts(prev => prev.filter(t => t.id !== id));
        }, 4000);
    }, []);

    return {toasts, showToast};
};