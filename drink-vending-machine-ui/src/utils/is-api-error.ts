import {ApiError} from "@/app/api/drink-vending-machine/core/ApiError";

type ProblemDetails = {
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
};

export function isUnableToGiveChangeError(error: unknown): boolean {
    if (error instanceof ApiError) {
        const body = error.body as ProblemDetails;
        if (!body) return false;
        if (
            body.status === 400 &&
            body.title === "Operation is not valid" &&
            body.detail?.toLowerCase().includes("unable to give change")
        ) {
            return true;
        }
    }
    return false;
}

