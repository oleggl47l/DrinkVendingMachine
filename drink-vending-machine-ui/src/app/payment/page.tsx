'use client';

import {useOrderContext} from "@/context/order-context";
import {usePayment} from "@/hooks/payment/use-payment";
import {PaymentSummary} from "@/components/payment/payment-summary";
import {useState} from "react";
import {usePaymentHandler} from "@/hooks/payment/use-payment-handler";
import {usePaymentRedirect} from "@/hooks/payment/use-payment-redirect";
import {PaymentControls} from "@/components/payment/payment-controls";
import {CoinList} from "@/components/payment/coin-list";

export default function PaymentPage() {
    const {total: orderTotal, orderItems, isHydrated} = useOrderContext();
    const {coins, totalInserted, changeCount, isLoaded} = usePayment();
    const [isPaid, setIsPaid] = useState(false);

    const {handlePayment, isEnough, loading} = usePaymentHandler(coins, totalInserted);
    usePaymentRedirect(isPaid);

    if (!isHydrated || orderItems.length === 0) return null;

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

            <CoinList coins={coins} changeCount={changeCount} isLoaded={isLoaded}/>

            <div className="mt-auto border-t border-gray-300 pt-6">
                <div className="flex justify-end mb-8">
                    <PaymentSummary
                        orderTotal={orderTotal}
                        totalInserted={totalInserted}
                        isEnough={isEnough}
                    />
                </div>

                <PaymentControls
                    loading={loading}
                    isEnough={isEnough}
                    onPay={async () => {
                        setIsPaid(true);
                        await handlePayment();
                    }}
                />
            </div>
        </div>
    );
}
