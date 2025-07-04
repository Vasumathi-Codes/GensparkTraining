# Use lightweight Node.js Alpine base image
FROM node:18-alpine

# Set working directory
WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

# Copy rest of the application code
COPY . .

# Expose port (Express runs on 5000)
EXPOSE 5000

# Start the server
CMD ["node", "index.js"]
