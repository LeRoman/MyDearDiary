FROM node:20.10.0 as build-env

WORKDIR /app

COPY . .
RUN npm install
RUN npm run build --configuration=production

FROM nginx:alpine
COPY --from=build-env /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf