'use client';

import {useEffect, useState} from "react";
import {useRouter} from "next/navigation";

export function usePaymentSuccess() {
    const router = useRouter();
    const [changeAmount, setChangeAmount] = useState<number | null>(null);
    const [changeCoins, setChangeCoins] = useState<Record<string, number>>({});

    useEffect(() => {
        const amount = sessionStorage.getItem('paymentChangeAmount');
        const coins = sessionStorage.getItem('paymentChangeCoins');

        if (amount !== null && coins !== null) {
            setChangeAmount(Number(amount));
            setChangeCoins(JSON.parse(coins));
        } else {
            router.replace('/catalog');
        }
    }, [router]);

    const clearStorage = () => {
        sessionStorage.removeItem('paymentChangeAmount');
        sessionStorage.removeItem('paymentChangeCoins');
    };

    return {changeAmount, changeCoins, clearStorage};
}
