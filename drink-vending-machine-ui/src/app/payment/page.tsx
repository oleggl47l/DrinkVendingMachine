'use client';

import {useOrderContext} from "@/context/order-context";
import {usePayment} from "@/hooks/use-payment";
import {CoinRow} from "@/components/payment/coin-row";
import {PaymentSummary} from "@/components/payment/payment-summary";
import {useEffect, useState} from "react";
import {useRouter} from "next/navigation";
import {OrderService} from "@/app/api/drink-vending-machine";

export default function PaymentPage() {
    const router = useRouter();
    const {total: orderTotal, orderItems, isHydrated, removeItem} = useOrderContext();
    const {coins, totalInserted, changeCount, isLoaded} = usePayment();
    const [loading, setLoading] = useState(false);
    const [isPaid, setIsPaid] = useState(false);

    useEffect(() => {
        if (!isHydrated) return;
        if (orderItems.length === 0 && !isPaid) {
            router.replace('/catalog');
        }
    }, [orderItems, isHydrated, router, isPaid]);

    if (!isHydrated || orderItems.length === 0) {
        return null;
    }

    const isEnough = totalInserted >= orderTotal;

    const handlePayment = async () => {
        setLoading(true);
        try {
            const orderCreateModel = {
                items: orderItems.map(item => ({
                    drinkId: item.id,
                    quantity: item.quantitySelected,
                })),
                coinsInserted: coins
                    .filter(c => c.countSelected > 0)
                    .map(c => ({
                        id: c.id,
                        quantity: c.countSelected,
                    })),
            };

            const result = await OrderService.createOrder({requestBody: orderCreateModel});

            orderItems.forEach(item => removeItem(item.id!));

            sessionStorage.setItem('paymentChangeAmount', String(result.changeAmount ?? 0));
            sessionStorage.setItem('paymentChangeCoins', JSON.stringify(result.change ?? {}));

            setIsPaid(true);
            router.push('/payment-success');
        } catch (error) {
            alert('Ошибка при оплате, попробуйте ещё раз');
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

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
                    onClick={handlePayment}
                    disabled={!isEnough || loading}
                    className="bg-green-600 text-white hover:bg-green-700 px-20 py-3 rounded disabled:opacity-50"
                >
                    {loading ? 'Оплата...' : 'Оплатить'}
                </button>
            </div>
        </div>
    );
}
