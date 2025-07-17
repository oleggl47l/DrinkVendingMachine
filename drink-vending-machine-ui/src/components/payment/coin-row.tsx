'use client';

import React from 'react';
import {CoinWithCount} from "@/hooks/use-payment";

interface CoinRowProps {
    coin: CoinWithCount;
    onChange: (id: number | undefined, delta: number) => void;
}

export const CoinRow = ({coin, onChange}: CoinRowProps) => {
    return (
        <div className="grid grid-cols-[1fr_328px_100px] gap-4 items-center mb-12">
            <div className="flex items-center gap-4">
                <div
                    className="w-12 h-12 rounded-full border-2 border-gray-600 bg-gray-300 text-gray-800 flex items-center justify-center font-semibold text-lg"
                >
                    {coin.nominal}
                </div>
                <span>{coin.nominal} {getRubleWord(coin.nominal)}</span>
            </div>

            <div className="flex items-center gap-2">
                <button
                    onClick={() => onChange(coin.id, -1)}
                    className="bg-black text-white w-8 h-8 flex items-center justify-center rounded"
                    disabled={coin.countSelected <= 0}
                >
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"
                         className="lucide lucide-minus-icon lucide-minus">
                        <path d="M5 12h14"/>
                    </svg>
                </button>
                <div className="w-16 h-8 flex items-center justify-center border rounded">
                    {coin.countSelected}
                </div>
                <button
                    onClick={() => onChange(coin.id, 1)}
                    className="bg-black text-white w-8 h-8 flex items-center justify-center rounded"
                >
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                         stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"
                         className="lucide lucide-plus-icon lucide-plus">
                        <path d="M5 12h14"/>
                        <path d="M12 5v14"/>
                    </svg>
                </button>
            </div>

            <div>{(coin.nominal || 0) * coin.countSelected} руб.</div>
        </div>
    );
};

function getRubleWord(nominal?: number) {
    if (!nominal) return 'руб.';
    const last = nominal % 10;
    const lastTwo = nominal % 100;
    if (last === 1 && lastTwo !== 11) return 'рубль';
    if ([2, 3, 4].includes(last) && ![12, 13, 14].includes(lastTwo)) return 'рубля';
    return 'рублей';
}
