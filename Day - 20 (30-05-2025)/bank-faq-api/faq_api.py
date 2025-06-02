# filename: faq_api.py
from fastapi import FastAPI
from pydantic import BaseModel
from transformers import pipeline

app = FastAPI()
qa_pipeline = pipeline("question-answering", model="distilbert-base-uncased-distilled-squad")

faq_context = """
Net banking allows you to access your bank account online. You can check balances, transfer funds, and manage finances anytime.

To reset your net banking password, use the 'Forgot Password' option and follow the verification steps.

We offer savings, current, fixed deposit, and recurring deposit accounts. Each has unique benefits depending on your needs.

Credit cards come with cashback, reward points, secure transactions, and EMI options for easier purchases.

We provide loans like personal loans, home loans, vehicle loans, and business loans with competitive interest rates and flexible repayment.

Apply for loans via our website or mobile app by uploading documents and filling the form. Our support team helps if needed.

Download your bank statement via the 'Statements' section in net banking or the mobile app.

We offer webinars on budgeting, investing, saving, and online banking safety.

We use encryption and 2FA (two-factor authentication) for secure banking. Never share OTPs or passwords with anyone.
"""

class QuestionRequest(BaseModel):
    question: str

@app.post("/ask")
def ask_question(request: QuestionRequest):
    try:
        result = qa_pipeline(question=request.question, context=faq_context)
        return {"answer": result['answer']}
    except Exception:
        return {"answer": "Sorry, I couldn't find an answer."}

