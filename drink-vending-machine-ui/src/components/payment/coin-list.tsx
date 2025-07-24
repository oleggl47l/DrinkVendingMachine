import {CoinRow} from "@/components/payment/coin-row";
import {Loading} from "@/components/ui/loading";
import {CoinWithCount} from "@/hooks/payment/use-payment";

interface Props {
    coins: CoinWithCount[];
    changeCount: (id: number | undefined, delta: number) => void;
    isLoaded: boolean;
}

export const CoinList = ({coins, changeCount, isLoaded}: Props) => {
    if (!isLoaded) return <Loading/>;

    return (
        <section
            className="flex-grow overflow-y-auto space-y-4 pr-4 min-h-[100px]"
            style={{scrollbarGutter: 'stable'}}
            aria-label="Монеты для оплаты"
        >
            {coins.map(coin => (
                <CoinRow key={coin.id} coin={coin} onChange={changeCount}/>
            ))}
        </section>
    );
};