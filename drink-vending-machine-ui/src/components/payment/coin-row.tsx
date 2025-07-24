'use client';

import React from 'react';
import {CoinWithCount} from "@/hooks/payment/use-payment";
import {useCoinCount} from "@/hooks/payment/use-coin-count";
import {getRubleWord} from "@/utils/get-ruble-word";

interface CoinRowProps {
    coin: CoinWithCount;
    onChange: (id: number | undefined, delta: number) => void;
}

export const CoinRow = ({coin, onChange}: CoinRowProps) => {
    const {
        inputValue,
        handleInputChange,
        handleBlur,
        increment,
        decrement
    } = useCoinCount(coin.countSelected, (delta) => onChange(coin.id, delta));

    return (
        <div className="grid grid-cols-[1fr_2fr_1fr] gap-4 items-center mb-8">
            <div className="flex items-center gap-4">
                <div className="w-12 h-12 rounded-full border-2 border-gray-600 bg-gray-300 text-gray-800 flex items-center justify-center font-semibold text-lg">
                    {coin.nominal}
                </div>
                <span>{coin.nominal} {getRubleWord(coin.nominal ?? 0)}</span>
            </div>

            <div className="flex justify-center items-center gap-2">
                <button
                    onClick={decrement}
                    className="bg-black text-white w-8 h-8 flex items-center justify-center rounded hover:bg-gray-700"
                    disabled={coin.countSelected <= 0}
                    aria-label={`Уменьшить количество монет номиналом ${coin.nominal}`}
                >
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"
                         className="lucide lucide-minus">
                        <path d="M5 12h14"/>
                    </svg>
                </button>
                <input
                    type="text"
                    inputMode="numeric"
                    pattern="[0-9]*"
                    value={inputValue}
                    onChange={(e) => handleInputChange(e.target.value)}
                    onBlur={handleBlur}
                    className="w-16 h-8 text-center border rounded appearance-none"
                    aria-label={`Количество монет номиналом ${coin.nominal}`}
                />
                <button
                    onClick={increment}
                    className="bg-black text-white w-8 h-8 flex items-center justify-center rounded hover:bg-gray-700"
                    aria-label={`Увеличить количество монет номиналом ${coin.nominal}`}
                >
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"
                         className="lucide lucide-plus">
                        <path d="M5 12h14"/>
                        <path d="M12 5v14"/>
                    </svg>
                </button>
            </div>

            <div className="flex justify-center font-semibold">
                {(coin.nominal ?? 0) * coin.countSelected} руб.
            </div>
        </div>
    );
};
