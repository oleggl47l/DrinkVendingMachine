import {OrderTotal} from "@/components/order/order-total";

interface Props {
    total: number;
    isDisabled: boolean;
    onGoBack: () => void;
    onPay: () => void;
}

export const OrderSummary = ({isDisabled, onGoBack, onPay}: Props) => (
    <div className="flex justify-between">
        <button
            onClick={onGoBack}
            className="bg-yellow-400 text-black hover:bg-yellow-500 px-20 py-3 rounded"
        >
            Вернуться
        </button>
        <button
            onClick={onPay}
            disabled={isDisabled}
            className="bg-green-600 text-white hover:bg-green-700 px-20 py-3 rounded disabled:bg-green-600 disabled:opacity-50"
        >
            Оплата
        </button>
    </div>
);