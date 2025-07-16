'use client';

import {useDrinks} from "@/hooks/use-drinks";
import {DrinkFilters} from "@/components/drinks/drink-filter";
import {DrinkList} from "@/components/drinks/drink-list";

export default function Home() {
    const {
        drinks,
        brands,
        selectedBrand,
        priceBounds,
        selectedRange,
        loading,
        handleBrandChange,
        handlePriceChange
    } = useDrinks();

    return (
        <div className="container mx-auto px-4 py-8">
            <h1 className="text-3xl font-bold mb-8">Автомат с напитками</h1>

            <DrinkFilters
                brands={brands}
                selectedBrand={selectedBrand}
                priceBounds={priceBounds}
                selectedRange={selectedRange}
                onBrandChange={handleBrandChange}
                onPriceChange={handlePriceChange}
            />

            <DrinkList drinks={drinks} loading={loading} />
        </div>
    );
}