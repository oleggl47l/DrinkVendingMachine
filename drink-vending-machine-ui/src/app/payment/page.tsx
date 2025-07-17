'use client';

import {useOrderContext} from "@/context/order-context";
import {usePayment} from "@/hooks/use-payment";
import {CoinRow} from "@/components/payment/coin-row";
import {PaymentSummary} from "@/components/payment/payment-summary";
import {useEffect} from "react";
import {useRouter} from "next/navigation";

export default function PaymentPage() {
    const router = useRouter();
    const {total: orderTotal, orderItems, isHydrated} = useOrderContext();
    const {coins, totalInserted, changeCount, isLoaded} = usePayment();

    useEffect(() => {
        if (!isHydrated) return;
        if (orderItems.length === 0) {
            router.replace('/catalog');
        }
    }, [orderItems, isHydrated, router]);

    if (!isHydrated || orderItems.length === 0) {
        return null;
    }

    const isEnough = totalInserted >= orderTotal;

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold mb-6">Оплата</h1>

            <section className="grid grid-cols-[1fr_304px_100px] gap-4 font-semibold mb-4">
                <div>Номинал</div>
                <div>Количество</div>
                <div>Сумма</div>
            </section>

            <section className="border-t border-gray-300 pt-4">
                {!isLoaded ? (
                    <p>Загрузка...</p>
                ) : (
                    coins.map(coin => (
                        <CoinRow key={coin.id} coin={coin} onChange={changeCount}/>
                    ))
                )}
            </section>

            <PaymentSummary
                orderTotal={orderTotal}
                totalInserted={totalInserted}
                isEnough={isEnough}
            />

            <div className="flex justify-between mt-8">
                <button
                    onClick={() => window.history.back()}
                    className="bg-yellow-400 text-black hover:bg-yellow-500 px-20 py-3 rounded"
                >
                    Вернуться
                </button>
                <button
                    onClick={() => alert('Оплата не реализована')}
                    disabled={!isEnough}
                    className="bg-green-600 text-white hover:bg-green-700 px-20 py-3 rounded disabled:opacity-50"
                >
                    Оплатить
                </button>
            </div>
        </div>
    );
}
