'use client';

import {useEffect, useState} from 'react';
import {CoinModel, CoinService} from "@/app/api/drink-vending-machine";

export type CoinWithCount = CoinModel & { countSelected: number };

export function usePayment() {
    const [coins, setCoins] = useState<CoinWithCount[]>([]);
    const [totalInserted, setTotalInserted] = useState(0);
    const [isLoaded, setIsLoaded] = useState(false);

    useEffect(() => {
        CoinService.getAllCoins().then(data => {
            const withCount = data.map(coin => ({
                ...coin,
                countSelected: 0,
            }));
            setCoins(withCount);
            setIsLoaded(true);
        });
    }, []);

    useEffect(() => {
        const sum = coins.reduce((acc, c) => acc + (c.nominal || 0) * c.countSelected, 0);
        setTotalInserted(sum);
    }, [coins]);

    const changeCount = (id: number | undefined, delta: number) => {
        if (id === undefined) return;
        setCoins(prev =>
            prev.map(c => {
                if (c.id === id) {
                    const newCount = c.countSelected + delta;
                    if (newCount < 0) return c;
                    return {...c, countSelected: newCount};
                }
                return c;
            })
        );
    };

    return {
        coins,
        totalInserted,
        isLoaded,
        changeCount,
    };
}
