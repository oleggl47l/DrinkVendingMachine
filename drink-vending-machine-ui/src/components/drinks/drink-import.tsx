'use client';

import {useDrinkImport} from "@/hooks/catalog/use-drink-import";

interface Props {
    onImportSuccess?: () => void;
}

export const DrinkImport = ({onImportSuccess}: Props) => {
    const {
        uploading,
        error,
        fileInputRef,
        handleImport
    } = useDrinkImport(onImportSuccess);

    return (
        <div className="flex flex-col">
            <button
                onClick={() => fileInputRef.current?.click()}
                className="bg-gray-200 text-gray-800 px-4 py-3 rounded disabled:opacity-50 hover:bg-gray-300"
                disabled={uploading}
            >
                {uploading ? "Импортируется..." : "Импорт"}
            </button>
            <input
                type="file"
                ref={fileInputRef}
                accept=".xlsx,.xls"
                className="hidden"
                onChange={(e) => handleImport(e.target.files?.[0] || null)}
            />
            {error && <span className="text-red-500 text-sm mt-2">{error}</span>}
        </div>
    );
};
