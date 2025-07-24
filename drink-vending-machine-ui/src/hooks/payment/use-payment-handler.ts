import {useState} from "react";
import {useRouter} from "next/navigation";
import {useOrderContext} from "@/context/order-context";
import {OrderService} from "@/app/api/drink-vending-machine";
import {useToast} from "@/components/ui/toast";
import {isUnableToGiveChangeError} from "@/utils/is-api-error";
import {CoinWithCount} from "@/hooks/payment/use-payment";

export function usePaymentHandler(coins: CoinWithCount[], totalInserted: number) {
    const {orderItems, removeItem} = useOrderContext();
    const {showToast} = useToast();
    const [loading, setLoading] = useState(false);
    const router = useRouter();

    const isEnough = totalInserted >= orderItems.reduce((acc, item) => acc + (item.price || 0) * item.quantitySelected, 0);

    const handlePayment = async () => {
        if (!isEnough || loading) return;

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

            router.push('/payment-success');
        } catch (error) {
            if (isUnableToGiveChangeError(error)) {
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

    return {handlePayment, isEnough, loading};
}
