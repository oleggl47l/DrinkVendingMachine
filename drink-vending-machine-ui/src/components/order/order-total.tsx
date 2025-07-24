interface OrderTotalProps {
    total: number;
}

export const OrderTotal = ({ total }: OrderTotalProps) => (
    <div className="flex justify-end mt-2 mb-8">
        <p>Итоговая сумма</p>
        <p className="ml-6 text-xl font-semibold">{total} руб.</p>
    </div>
);