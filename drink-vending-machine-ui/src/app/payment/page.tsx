'use client';

import {useOrderContext} from "@/context/order-context";
import {usePayment} from "@/hooks/use-payment";
import {CoinRow} from "@/components/payment/coin-row";
import {PaymentSummary} from "@/components/payment/payment-summary";
import {useEffect, useState} from "react";
import {useRouter} from "next/navigation";
import {OrderService} from "@/app/api/drink-vending-machine";
import {Loading} from "@/components/ui/loading";
import {useToast} from "@/components/ui/toast";
import {isApiError} from "@/utils/is-api-error";

export default function PaymentPage() {
    const router = useRouter();
    const {total: orderTotal, orderItems, isHydrated, removeItem} = useOrderContext();
    const {coins, totalInserted, changeCount, isLoaded} = usePayment();
    const [loading, setLoading] = useState(false);
    const [isPaid, setIsPaid] = useState(false);
    const { showToast } = useToast();

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
            if (isApiError(error, 'UnableToGiveChange')) {
                showToast(
                    'Извините, в данный момент мы не можем продать вам товар по причине того, что автомат не может выдать вам нужную сдачу.',
                    'error'
                );
            } else {
                showToast('Ошибка при оплате, попробуйте ещё раз', 'error');
            }


        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container mx-auto px-4 py-8 flex flex-col h-[100vh] max-h-[100vh]">
            <h1 className="text-3xl font-bold mb-6 flex-shrink-0">Оплата</h1>

            <div className="flex-shrink-0 border-b border-gray-300 pb-4 mb-4 pr-8">
                <section className="grid grid-cols-[1fr_2fr_1fr] gap-4 font-semibold text-gray-700">
                    <div>Номинал</div>
                    <div className="flex justify-center">Количество</div>
                    <div className="flex justify-center">Сумма</div>
                </section>
            </div>

            <section
                className="flex-grow overflow-y-auto space-y-4 pr-4 min-h-[100px]"
                style={{ scrollbarGutter: 'stable' }}
                aria-label="Монеты для оплаты"
            >
                {!isLoaded ? (
                    <Loading />
                ) : (
                    coins.map(coin => (
                        <CoinRow key={coin.id} coin={coin} onChange={changeCount}/>
                    ))
                )}
            </section>

            <div className="mt-auto border-t border-gray-300 pt-6">
                <div className="flex justify-end mb-8">
                    <PaymentSummary
                        orderTotal={orderTotal}
                        totalInserted={totalInserted}
                        isEnough={isEnough}
                    />
                </div>

                <div className="flex justify-between">
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
        </div>
    );
}
