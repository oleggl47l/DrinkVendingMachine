'use client';

export const LockedScreen = ({onRefreshAAction}: { onRefreshAAction?: () => void }) => {
    return (
        <div className="h-screen flex flex-col items-center justify-center text-center gap-6">
            <div className="max-w-md">
                <h2 className="text-2xl font-bold mb-4">Автомат занят</h2>
                <p className="text-gray-700 mb-6">
                    Извините, в данный момент автомат используется другим пользователем.
                    Пожалуйста, попробуйте позже.
                </p>
                <button
                    onClick={onRefreshAAction}
                    className="bg-yellow-400 px-6 py-3  text-black rounded hover:bg-yellow-500 transition"

                >
                    Проверить снова
                </button>
            </div>
        </div>
    );
};