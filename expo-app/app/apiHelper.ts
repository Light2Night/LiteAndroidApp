import {API_CONTROLLERS_ROTE, API_IMAGES_ROTE, API_URL} from "@/app/envValues";

export function getActionUrl(controller: string, action: string) {
    return `${API_URL}/${API_CONTROLLERS_ROTE}/${controller}/${action}`;
}

export function getImageUrl(imageName: string, size: number) {
    return `${API_URL}/${API_IMAGES_ROTE}/${size}_${imageName}`;
}
