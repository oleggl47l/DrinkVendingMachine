import {useRouter} from "next/navigation";
import {useToast} from "@/components/ui/toast";
import {OrderItem} from "@/context/order-context";

export const useGoToPayment = (orderItems: OrderItem[]) => {
    const router = useRouter();
    const {showToast} = useToast();

    const goToPayment = () => {
        if (orderItems.length === 0) {
            showToast('Корзина пуста. Добавьте товары, чтобы перейти к оплате.', 'error');
            return;
        }

        router.push('/payment');
    };

    return { goToPayment };
};