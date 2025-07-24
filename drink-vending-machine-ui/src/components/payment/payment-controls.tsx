interface PaymentControlsProps {
    loading: boolean;
    isEnough: boolean;
    onPay: () => void;
}

export const PaymentControls = ({loading, isEnough, onPay}: PaymentControlsProps) => {
    return (
        <div className="flex justify-between">
            <button
                onClick={() => window.history.back()}
                className="bg-yellow-400 text-black hover:bg-yellow-500 px-20 py-3 rounded"
            >
                Вернуться
            </button>
            <button
                onClick={onPay}
                disabled={!isEnough || loading}
                className="bg-green-600 text-white hover:bg-green-700 px-20 py-3 rounded disabled:bg-green-600 disabled:opacity-50"
            >
                {loading ? 'Оплата...' : 'Оплатить'}
            </button>
        </div>
    );
};