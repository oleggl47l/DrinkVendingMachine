'use client';

interface Props {
    orderTotal: number;
    totalInserted: number;
    isEnough: boolean;
}

export const PaymentSummary = ({orderTotal, totalInserted, isEnough}: Props) => {
    return (
        <div className="border-t border-gray-300 mt-8 pt-4 flex justify-end gap-4 items-center">
            <div className="flex justify-end mr-12">
                <p>Итоговая сумма</p>
                <p className="ml-6 text-xl font-semibold">{orderTotal} руб.</p>
            </div>
            <p>
                Вы внесли{' '}
                <span className={isEnough ? '' : 'text-red-600 font-semibold ml-6'}>
                    {totalInserted} руб.
                </span>{' '}
            </p>
        </div>
    );
};
