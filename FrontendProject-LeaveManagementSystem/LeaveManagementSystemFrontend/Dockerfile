# Stage 1: Build Angular App using Node 22
FROM node:22 AS builder

WORKDIR /app

COPY package*.json ./
RUN npm install --legacy-peer-deps

COPY . .
RUN npm run build -- --configuration production --project LeaveManagementSystemFrontend

# Stage 2: Serve App using NGINX
FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*
COPY --from=builder /app/dist/LeaveManagementSystemFrontend/browser /usr/share/nginx/html

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]



# Stage 1: Build Angular App
FROM node:20-alpine AS builder

WORKDIR /app

COPY package.json package-lock.json ./
RUN npm install --legacy-peer-deps

COPY . .
RUN npm run build -- --configuration production

# Stage 2: Serve with NGINX
FROM nginx:1.25-alpine

# Clean default NGINX content
RUN rm -rf /usr/share/nginx/html/*

# Copy Angular output to NGINX public folder
COPY --from=builder /app/dist/PropZone/browser /usr/share/nginx/html

# Optional: custom NGINX config (for Angular routing fallback)
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]