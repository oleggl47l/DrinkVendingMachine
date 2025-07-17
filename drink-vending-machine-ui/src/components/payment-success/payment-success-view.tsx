'use client';

import React from "react";

interface Props {
    changeAmount: number;
    changeCoins: Record<string, number>;
    onReturnToCatalog: () => void;
}

export const PaymentSuccessView: React.FC<Props> = ({
                                                        changeAmount,
                                                        changeCoins,
                                                        onReturnToCatalog,
                                                    }) => {
    return (
        <div
            className="container mx-auto px-4 py-8 flex flex-col justify-center items-center min-h-[100vh] text-center">
            <h1 className="text-3xl font-bold mb-4">Спасибо за покупку!</h1>

            <p className="mb-8 text-3xl">
                Пожалуйста, возьмите вашу сдачу:{" "}
                <span className="text-green-600 font-semibold">{changeAmount} руб</span>
            </p>

            <div className="mb-8">
                <h2 className="text-xl font-semibold mb-6">Ваши монеты:</h2>
                <div className="flex flex-col items-center gap-4">
                    {Object.entries(changeCoins).length > 0 ? (
                        Object.entries(changeCoins)
                            .map(([nominal, count]) => (
                                <div
                                    key={nominal}
                                    className="flex items-center justify-between gap-6 text-lg mb-6"
                                >
                                    <div className="flex items-center gap-4">
                                        <div
                                            className="w-12 h-12 rounded-full border-2 border-gray-600 bg-gray-300 text-gray-800 flex items-center justify-center font-semibold text-lg">
                                            {nominal}
                                        </div>
                                    </div>
                                    <div className="text-lg">
                                        {count}шт.
                                    </div>
                                </div>
                            ))
                    ) : (
                        <p>Сдачи нет</p>
                    )}
                </div>
            </div>

            <button
                onClick={onReturnToCatalog}
                className="bg-yellow-400 px-6 py-3 text-black rounded hover:bg-yellow-500 transition"
            >
                Каталог напитков
            </button>
        </div>
    );
};
