# Etapa 1: Build
FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build -- --configuration production

# Etapa 2: Serve con Nginx
FROM nginx:alpine
# Copiamos el build de Angular a la carpeta de Nginx
# Ojo: Asegúrate de que la ruta 'dist/backoffice-viajes-altairis/browser' sea la correcta en tu proyecto
COPY --from=build /app/dist/backoffice-viajes-altairis/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]