'use client';

import React, {createContext, useContext, useState, useCallback, ReactNode} from 'react';

interface Toast {
    id: number;
    message: string;
    type: 'success' | 'error';
    leaving?: boolean;
}

interface ToastContextType {
    showToast: (message: string, type: 'success' | 'error') => void;
}

const ToastContext = createContext<ToastContextType | undefined>(undefined);

export const ToastProvider = ({children}: { children: ReactNode }) => {
    const [toasts, setToasts] = useState<Toast[]>([]);

    const showToast = useCallback((message: string, type: 'success' | 'error') => {
        const id = Date.now();
        setToasts((prev) => [...prev, {id, message, type}]);

        setTimeout(() => {
            setToasts((prev) =>
                prev.map(t => (t.id === id ? {...t, leaving: true} : t))
            );
        }, 3600);

        setTimeout(() => {
            setToasts((prev) => prev.filter(t => t.id !== id));
        }, 4000);
    }, []);

    return (
        <ToastContext.Provider value={{showToast}}>
            {children}
            <div className="fixed bottom-5 right-5 flex flex-col gap-3 z-50">
                {toasts.map(({id, message, type, leaving}) => (
                    <div
                        key={id}
                        className={`
              max-w-xs px-4 py-3 rounded shadow-lg text-white 
              ${type === 'success' ? 'bg-green-600' : 'bg-red-600'}
              ${leaving ? 'animate-slideOut' : 'animate-slideIn'}
            `}
                        style={{animationDuration: '0.4s', opacity: 0.85}}
                    >
                        {message}
                    </div>
                ))}
            </div>

            <style jsx>{`
                @keyframes slideIn {
                    0% {
                        opacity: 0;
                        transform: translateX(100%);
                    }
                    100% {
                        opacity: 1;
                        transform: translateX(0);
                    }
                }
                @keyframes slideOut {
                    0% {
                        opacity: 1;
                        transform: translateX(0);
                    }
                    100% {
                        opacity: 0;
                        transform: translateX(100%);
                    }
                }
                .animate-slideIn {
                    animation-name: slideIn;
                    animation-timing-function: ease-out;
                }
                .animate-slideOut {
                    animation-name: slideOut;
                    animation-timing-function: ease-in;
                }
            `}</style>
        </ToastContext.Provider>
    );
};

export const useToast = () => {
    const context = useContext(ToastContext);
    if (!context) throw new Error('useToast must be used within a ToastProvider');
    return context;
};